import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/product';
import { ProductDetailsService } from './product-details.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from '../basket/basket.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  quantity = 1;
  quantityInBasket = 0;

  constructor(
    private productDetailsService: ProductDetailsService,
    private activatedRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService
  ) {
    this.breadcrumbService.set('@productDetails', '');
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id)
      this.productDetailsService.getProduct(+id).subscribe({
        next: (response) => {
          this.product = response;
          this.breadcrumbService.set('@productDetails', this.product.name);
          this.basketService.basketSource$.pipe(take(1)).subscribe({
            next: (basket) => {
              const item = basket?.items.find((i) => i.id === +id);
              if (item) {
                this.quantity = item.quantity;
                this.quantityInBasket = item.quantity;
              }
            },
          });
        },
        error: (err) => console.log(err),
      });
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    this.quantity--;
  }

  updateBasket() {
    if (this.product) {
      if (this.quantity > this.quantityInBasket) {
        const itemToAdd = this.quantity - this.quantityInBasket;
        this.quantityInBasket += itemToAdd;
        this.basketService.addItemToBasket(this.product, itemToAdd);
      } else {
        const itemToRemove = this.quantityInBasket - this.quantity;
        this.quantityInBasket -= itemToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemToRemove);
      }
    }
  }
  get buttonText() {
    return this.quantityInBasket === 0 ? 'Add to basket' : 'Update basket';
  }
}
