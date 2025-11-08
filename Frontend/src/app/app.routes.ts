import { Routes } from '@angular/router';
import { HomepageComponent } from '@components/Homepage/homepage.component';
import { ArtistListComponent } from '@components/artist-list/artist-list.component';
import { ArtistDetailsComponent } from '@components/artist-details/artist-details.component';
import { HappeningListComponent } from '@components/happening-list/happening-list.component';
import { HappeningCreateFormComponent } from '@components/happening-create-form/happening-create-form.component';
import { HappeningDetailComponent } from '@components/happening-detail/happening-detail.component';
import { ContactFormComponent } from '@components/contact-form/contact-form.component';
import { RegisterComponent } from '@components/auth-register/auth-register.component';
import { LoginComponent } from '@components/auth-login/auth-login.component';
import { NotFound404Component } from '@components/404/404-not-found';

export const routes: Routes = [
  { path: '', component: HomepageComponent, pathMatch: 'full' },
  { path: 'artist', component: ArtistListComponent },
  { path: 'artist/:guid', component: ArtistDetailsComponent },
  { path: 'artist/:guid/:name', component: ArtistDetailsComponent },
  { path: 'happening', component: HappeningListComponent },
  { path: 'happening/add', component: HappeningCreateFormComponent },
  { path: 'happening/:slug', component: HappeningDetailComponent },
  { path: 'contact', component: ContactFormComponent },
  { path: 'account/register', component: RegisterComponent },
  { path: 'account/login', component: LoginComponent},
  { path: '404', component: NotFound404Component },
  { path: '**', redirectTo: '' }  //falback
];
