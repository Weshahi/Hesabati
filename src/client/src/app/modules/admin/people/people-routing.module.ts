import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './components/customer/customer.component';
import { SystemUserComponent } from './components/system-user/system-user.component';


const routes: Routes = [
  {
    path: 'customers',
    component: CustomerComponent
  },
  {
    path: 'users',
    component: SystemUserComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PeopleRoutingModule { }
