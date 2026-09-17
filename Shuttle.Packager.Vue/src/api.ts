import axios, { type AxiosInstance } from "axios";
import { useAlertStore } from "@/stores/alert";
import configuration from "./configuration";
import { i18n } from "@/i18n";
import type { ProblemDetails } from "@/packager";

const getErrorMessage = (error: any): string => {
  const data = error.response?.data;

  if (data && typeof data === "object") {
    const problemDetails = data as ProblemDetails;

    return problemDetails.detail || problemDetails.title || JSON.stringify(data);
  }

  return (
    data ||
    error.response?.statusText ||
    "(unknown communication/network error)"
  );
};

const configure = (api: AxiosInstance): AxiosInstance => {
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      const alertStore = useAlertStore();

      if (error.response?.status === 401) {
        alertStore.add({
          message: i18n.global.t("exceptions.unauthorized"),
          type: "error",
          name: "api-error",
        });

        return error;
      }

      alertStore.add({
        message: getErrorMessage(error),
        type: "error",
        name: "api-error",
      });

      return Promise.reject(error);
    },
  );

  return api;
};

const api = configure(axios.create({ baseURL: configuration.getUrl() }));

export { api };
