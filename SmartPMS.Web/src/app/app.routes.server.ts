import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Auth pages are purely client-side — SSR/Prerender breaks form state and HTTP callbacks
  {
    path: 'login',
    renderMode: RenderMode.Client
  },
  {
    path: 'register',
    renderMode: RenderMode.Client
  },
  // All other routes also run on client (no backend data needed at prerender time)
  {
    path: '**',
    renderMode: RenderMode.Client
  }
];
