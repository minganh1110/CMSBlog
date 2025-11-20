import { Component, OnInit } from '@angular/core';

import { navItems } from './_nav';
import { TokenStorageService } from '../../shared/services/token-storage.service';
import { Router } from '@angular/router';
import { UrlConstants } from '../../shared/constants/url.constants';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent implements OnInit {
  public navItems = [];

  constructor(
    private tokenService: TokenStorageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    var user = this.tokenService.getUser();
    if (user == null) {
      this.router.navigate([UrlConstants.LOGIN]);
      return;
    }
    var navItemsCopy = JSON.parse(JSON.stringify(navItems));
    var permissions = user.permissions ? JSON.parse(user.permissions) : [];
    for (var index = 0; index < navItemsCopy.length; index++) {
       for (
        var childIndex = 0;
        childIndex < navItemsCopy[index].children?.length;
         childIndex++
       ) {
         if (
          navItemsCopy[index].children[childIndex].attributes &&
           permissions.filter(
            
            (x: any) =>
              x ==
              navItemsCopy[index].children[childIndex].attributes['policyName']
           ).length == 0
         ) {
          
          navItemsCopy[index].children[childIndex].class = 'hidden';
         }
       }
     }
    
    this.navItems = navItemsCopy;
   }
 }
