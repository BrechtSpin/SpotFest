import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavigationBarComponent } from '@components/navigation-bar/navigation-bar.component';
import { UserStatusComponent } from '@components/user-status/user-status.component'

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, NavigationBarComponent, UserStatusComponent],
  templateUrl: './header.component.html'
})
export class HeaderComponent {

}
