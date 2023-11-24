import { Injectable, inject } from '@angular/core';
import { CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs';
import { AccountService } from 'src/app/page/account/account.service';

@Injectable()
export class TokenService {
  constructor(private accountService: AccountService, private router: Router) {}

  canActivate(state: RouterStateSnapshot) {
    return this.accountService.currentUser$.pipe(
      map((auth) => {
        if (auth) return true;
        this.router.navigate(['account/login'], {
          queryParams: { returnUrl: state.url },
        });
        return false;
      })
    );
  }
}

export const authGuard: CanActivateFn = (route, state) => {
  return inject(TokenService).canActivate(state);
};
