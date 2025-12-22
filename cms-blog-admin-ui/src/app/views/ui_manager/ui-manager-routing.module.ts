import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MenuComponent } from './menu/menu.component';

const routes: Routes = [
    {
        path: '',
        data: {
            title: 'UI Manager'
        },
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'menu'
            },
            {
                path: 'menu',
                component: MenuComponent,
                data: {
                    title: 'Menu'
                }
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UiManagerRoutingModule {
}
