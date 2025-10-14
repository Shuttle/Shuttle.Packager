import type { Configuration, Env } from "./packager";
import axios from "axios";

let errorMessage: string;
let values: Env;
let isOk = true;

try {
  const env = async (): Promise<Env> => {
    if (import.meta.env.MODE === "production") {
      return (await axios.get<Env>("/env")).data;
    } else {
      return {
        VITE_API_URL: import.meta.env.VITE_API_URL,
      };
    }
  };

  values = await env();
} catch (error: any) {
  isOk = false;
  errorMessage = error.toString();
}

const getConfiguration = (): Configuration => {
  return {
    isOk() {
      return isOk;
    },
    getErrorMessage() {
      return errorMessage;
    },
    getUrl() {
      return isOk
        ? `${values.VITE_API_URL}${values.VITE_API_URL.endsWith("/") ? "" : "/"}`
        : "";
    },
    isDebugging() {
      return import.meta.env.DEV;
    },
    getApiUrl(path: string) {
      if (path.startsWith("/") && path.length < 2) {
        path = "";
      }

      return this.getUrl() + (path.startsWith("/") ? path.substring(1) : path);
    },
  };
};

const configuration = getConfiguration();

if (!import.meta.env.VITE_API_URL) {
  throw new Error("Configuration item 'VITE_API_URL' has not been set.");
}

if (Object.freeze) {
  Object.freeze(configuration);
}

export default configuration;
