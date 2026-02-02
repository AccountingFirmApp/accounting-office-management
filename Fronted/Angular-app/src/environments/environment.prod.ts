import { envConfig } from '../app/app.config.env.example';

export const environment = {
  production: true,
  apiUrl: envConfig.apiUrl,
  googleClientId: envConfig.googleClientId
};
