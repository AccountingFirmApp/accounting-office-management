import { envConfig } from '../app/app.config.env.example';

export const environment = {
  production: false,
  apiUrl: envConfig.apiUrl,
  googleClientId: envConfig.googleClientId
};
