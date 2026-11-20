import { Routes } from '@angular/router';
import { authGuard, guestGuard } from './core/auth.guard';
import { AppShell } from './layout/app-shell';
import { Login } from './pages/login/login';
import { TrackDetail } from './pages/track-detail/track-detail';
import { TrackList } from './pages/track-list/track-list';

export const routes: Routes = [
  { path: 'login', component: Login, canActivate: [guestGuard] },
  {
    path: '',
    component: AppShell,
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'tracks' },
      { path: 'tracks', component: TrackList },
      { path: 'tracks/:id', component: TrackDetail },
    ],
  },
  { path: '**', redirectTo: 'tracks' },
];
