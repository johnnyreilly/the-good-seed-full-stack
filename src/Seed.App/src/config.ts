export interface IConfig {
    apiBaseUrl: string;
    appBaseUrl: string;
    loginAppBaseUrl: string;
    loginApiBaseUrl: string;
    development: boolean;
}

export const config: IConfig = {
    apiBaseUrl: process.env.API_BASE_URL,
    appBaseUrl: process.env.APP_BASE_URL,
    loginAppBaseUrl: process.env.LOGIN_APP_BASE_URL,
    loginApiBaseUrl: process.env.LOGIN_API_BASE_URL,
    development: process.env.NODE_ENV !== 'production',
};