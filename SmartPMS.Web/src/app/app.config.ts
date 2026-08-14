import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors, withFetch } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // Removed withEventReplay() — it was replaying events during hydration
    // and resetting component in-memory state (errorMessage, successMessage) back to ''
    provideClientHydration(),
    // withFetch() ensures HTTP uses native fetch API (more compatible with SSR)
    provideHttpClient(withFetch(), withInterceptors([authInterceptor]))
  ]
};
