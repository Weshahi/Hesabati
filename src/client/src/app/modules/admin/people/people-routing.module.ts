import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from 'src/app/core/guards/permission.guard';
import { CustomerComponent } from './components/customer/customer.component';
import { SystemUserComponent } from './components/system-user/system-user.component';


const routes: Routes = [
  {
    path: 'customers',
    component: CustomerComponent
  },
  {
    path: 'users',
    component: SystemUserComponent,
    canActivate: [PermissionGuard],
    data: {
      allowedPermissions: ['Permissions.Users.View']
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PeopleRoutingModule { }
