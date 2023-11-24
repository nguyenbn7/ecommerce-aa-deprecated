import { Component } from '@angular/core';
import { BasketService } from 'src/app/page/basket/basket.service';
import { BasketItem } from '../../shared/basket';
import { AccountService } from 'src/app/page/account/account.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
})
export class NavBarComponent {
  constructor(
    public basketService: BasketService,
    public accountService: AccountService
  ) {}

  getCount(items: BasketItem[]) {
    return items.reduce((total, item) => total + item.quantity, 0);
  }
}
