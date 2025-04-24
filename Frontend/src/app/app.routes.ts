//import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { ArtistDetailsComponent } from './artist-details/artist-details.component'
import { HappeningFormComponent } from './happening/happening-form.component';

export const routes: Routes = [
  { path: 'artist/:id', component: ArtistDetailsComponent },
  { path: 'happening', component: HappeningFormComponent}
];

//@NgModule({
//  imports: [RouterModule.forRoot(routes)],
//  exports: [RouterModule]
//})
//export class AppRoutingModule { }
