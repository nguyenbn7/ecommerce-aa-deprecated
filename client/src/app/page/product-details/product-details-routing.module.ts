import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailsComponent } from './product-details.component';

const routes: Routes = [
  {
    path: ':id',
    component: ProductDetailsComponent,
    data: { breadcrumb: { alias: 'productDetails' } },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductDetailsRoutingModule {}
