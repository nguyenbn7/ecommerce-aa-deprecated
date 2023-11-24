import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import { ErrorsComponent } from './test/errors/errors.component';
import { NotFoundComponent } from './test/not-found/not-found.component';
import { ServerErrorComponent } from './test/server-error/server-error.component';
import { authGuard } from './cores/guards/auth-guard.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, data: { breadcrumb: 'Home' } },
  { path: 'test-errors', component: ErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  {
    path: 'shop',
    loadChildren: () =>
      import('./page/shop/shop.module').then((m) => m.ShopModule),
    data: { breadcrumb: { skip: true } },
  },
  {
    path: 'shop',
    loadChildren: () =>
      import('./page/product-details/product-details.module').then(
        (m) => m.ProductDetailsModule
      ),
  },
  {
    path: 'basket',
    loadChildren: () =>
      import('./page/basket/basket.module').then((m) => m.BasketModule),
    data: { breadcrumb: { skip: true } },
  },
  {
    path: 'checkout',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./page/checkout/checkout.module').then((m) => m.CheckoutModule),
    data: { breadcrumb: { skip: true } },
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./page/account/account.module').then((m) => m.AccountModule),
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
