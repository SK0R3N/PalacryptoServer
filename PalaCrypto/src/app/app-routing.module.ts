import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PalaCryptoComponent } from 'src/pala-crypto/pala-crypto.component';

const routes: Routes = [
  {path: 'Palacrypto', component: PalaCryptoComponent},
  {path: '**', redirectTo: '/Palacrypto'} 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
