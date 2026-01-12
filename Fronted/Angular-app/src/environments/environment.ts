// export const environment = {
//   production: false,
//   apiUrl: 'https://localhost:7118/' // ה-URL של ה-API שלך
// };


/**
 * הגדרות סביבת פיתוח (Development)
 * ⚠️ חשוב! וודאי שה-URL תואם לשרת שלך
 */
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7118/' // ✅ ה-URL של ה-API שלך
};

/**
 * הערות חשובות:
 * 
 * 1. אם השרת רץ על פורט אחר, שני אותו כאן
 * 2. אם אתה משתמש ב-HTTP (לא HTTPS), שני ל-http://
 * 3. וודא שיש / בסוף ה-URL
 * 4. בפרודקשן תצטרך קובץ environment.prod.ts עם ה-URL האמיתי
 * 
 * דוגמאות:
 * - פיתוח מקומי: 'https://localhost:7118/'
 * - פיתוח HTTP: 'http://localhost:5000/'
 * - פרודקשן: 'https://api.mycompany.com/'
 */