import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductItemComponent } from './components/product-item/product-item.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import { NgxSpinnerModule } from 'ngx-spinner';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TextInputComponentComponent } from './components/text-input-component/text-input-component.component';

@NgModule({
  declarations: [
    ProductItemComponent,
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent,
    TextInputComponentComponent,
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      progressBar: true,
      preventDuplicates: true,
    }),
    NgxSpinnerModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
  ],
  exports: [
    ProductItemComponent,
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    NgxSpinnerModule,
    OrderTotalsComponent,
    ReactiveFormsModule,
    BsDropdownModule,
    TextInputComponentComponent,
  ],
})
export class SharedModule {}
