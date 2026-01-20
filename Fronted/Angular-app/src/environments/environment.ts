import { envConfig } from '../app/app.config.env';

export const environment = {
  production: false,
  apiUrl: envConfig.apiUrl,
  googleClientId: envConfig.googleClientId
};
