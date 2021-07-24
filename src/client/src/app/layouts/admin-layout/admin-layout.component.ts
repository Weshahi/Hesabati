import { OverlayContainer } from '@angular/cdk/overlay';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'src/app/core/services/auth.service';
import { BusyService } from 'src/app/core/services/busy.service';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {

  title = 'Hesabati';
  fullName: string;
  email: string;

  constructor(private authService: AuthService, public busyService: BusyService) { }

  ngOnInit() {
    this.getUserDetails();
  }
  getUserDetails() {
    this.fullName = this.authService.getFullName;
    this.email = this.authService.getEmail;
  }
}
