import { envConfig } from '../app/app.config.env';

export const environment = {
  production: true,
  apiUrl: envConfig.apiUrl,
  googleClientId: envConfig.googleClientId
};
