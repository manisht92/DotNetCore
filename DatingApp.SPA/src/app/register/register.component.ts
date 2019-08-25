import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe((response: Response) => {
      console.log('Registration successful');
    }, error => {
      console.log('Failed to registered');
    });
    console.log(this.model);
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancel');
  }

}
