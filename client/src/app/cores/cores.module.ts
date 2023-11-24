import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { SharedModule } from '../shared/shared.module';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { RouterModule } from '@angular/router';
import { TokenService } from './guards/auth-guard.guard';

@NgModule({
  declarations: [NavBarComponent, SectionHeaderComponent],
  imports: [CommonModule, SharedModule, BreadcrumbModule, RouterModule],
  exports: [NavBarComponent, SectionHeaderComponent],
  providers: [TokenService],
})
export class CoresModule {}
