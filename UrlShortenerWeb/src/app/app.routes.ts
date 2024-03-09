import { Routes } from '@angular/router';
import {ShortenComponent} from "./components/shorten/shorten.component";
import {PageNotFoundComponent} from "./components/page-not-found/page-not-found.component";
import {ErrorPageComponent} from "./components/error-page/error-page.component";

export const routes: Routes = [
  { path: '',   redirectTo: '/shorten', pathMatch: 'full' },
  { path: 'shorten', component: ShortenComponent },
  { path: 'error', component: ErrorPageComponent },
  { path: '**', component: PageNotFoundComponent },
];
