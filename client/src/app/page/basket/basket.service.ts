import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket, BasketItem, BasketTotals } from 'src/app/shared/basket';
import { Product } from 'src/app/shared/product';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseURL = environment.apiURL;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();

  constructor(private httpClient: HttpClient) {}

  getBasket(id: string) {
    let params = new HttpParams();
    params = params.append('id', id);
    return this.httpClient
      .get<Basket>(this.baseURL + 'basket', { params })
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        },
      });
  }

  setBasket(basket: Basket) {
    return this.httpClient
      .post<Basket>(this.baseURL + 'basket', basket)
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        },
      });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find((i) => i.id === id);
    if (item) {
      item.quantity -= quantity;
      if (item.quantity <= 0) {
        basket.items = basket.items.filter((i) => i.id !== id);
      }
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.httpClient
      .delete(this.baseURL + 'basket?id=' + basket.id)
      .subscribe({
        next: () => {
          this.basketSource.next(null);
          this.basketTotalSource.next(null);
          localStorage.removeItem('basket_id');
        },
      });
  }

  private addOrUpdateItem(
    items: BasketItem[],
    itemToAdd: BasketItem,
    quantity: number
  ): BasketItem[] {
    const item = items.find((i) => i.id === itemToAdd.id);
    if (item) item.quantity += quantity;
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
    };
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const shipping = 0;
    const subtotal = basket.items.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, subtotal, total });
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }
}
