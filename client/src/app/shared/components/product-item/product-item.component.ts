import { Component, Input } from '@angular/core';
import { Product } from '../../product';
import { BasketService } from 'src/app/page/basket/basket.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css'],
})
export class ProductItemComponent {
  @Input({ required: true }) product!: Product;

  constructor(private basketSerivce: BasketService) {}

  addItemToBasket() {
    this.basketSerivce.addItemToBasket(this.product);
  }
}
