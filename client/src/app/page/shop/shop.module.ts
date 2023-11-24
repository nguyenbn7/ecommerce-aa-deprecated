import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShopComponent } from './shop.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ShopRoutingModule } from './shop-routing.module';

@NgModule({
  declarations: [ShopComponent],
  imports: [CommonModule, SharedModule, ShopRoutingModule],
})
export class ShopModule {}
