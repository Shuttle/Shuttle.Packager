export type Alert = {
  message: string;
  name: string;
  type?: "error" | "success" | "warning" | "info" | undefined;
  expire?: boolean;
  expirySeconds?: number;
  expiryDate?: Date;
  dismissable?: boolean;
  key?: string;
  variant?: string;
  visiblePercentage?: number;
};

export type AlertStoreState = {
  alerts: Alert[];
};

export type Configuration = {
  isOk: () => boolean;
  getErrorMessage: () => string;
  getUrl: () => string;
  isDebugging: () => boolean;
  getApiUrl: (path: string) => string;
};

export type ConfirmationOptions = {
  item?: any;
  onConfirm: (item?: any) => void;
  message?: string;
  title?: string;
};

export type ConfirmationStoreState = {
  isOpen: boolean;
  options?: ConfirmationOptions;
};

export type Env = {
  VITE_API_URL: string;
};

export type FormTitle = {
  title: string;
  closePath?: string;
  closeClick?: () => void;
  type?: "borderless" | "normal";
};

export type NugetVersion = {
  nugetVersion: string;
};

export type PackageOptions = {
  configuration: string;
  packageSourceName: string;
};

export type PackageResult = {
  log: string;
  failed: boolean;
};

export type PackageSource = {
  name: string;
};

export type Project = {
  id: string;
  name: string;
  path: string;
  version: string;
  vnext: string;
  editingVersion: boolean;
  selectable: boolean;
  busy: boolean;
  log: string;
  status: string;
  nugetVersion?: string;
};

export type ServerConfiguration = {
  allowPasswordAuthentication: boolean;
};

export type SnackbarStoreState = {
  visible: boolean;
  text: string;
  timeout: number;
};
