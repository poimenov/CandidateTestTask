export const config = () => {
    const baseUrl = import.meta.env.VITE_APP_BASE_URL;
    const serverURI = import.meta.env.VITE_REMOTE_SERVICE_BASE_URL;
    const api = import.meta.env.VITE_API;
  
    const isProd = import.meta.env.MODE === 'production';
    const isDev = import.meta.env.MODE === 'development';
  
    return {
      api: (endpoint: string) => `${serverURI}${api}${endpoint}`,
      baseUrl,
      isProd,
      isDev,
    };
  };