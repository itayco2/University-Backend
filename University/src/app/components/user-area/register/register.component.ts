import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserModel } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';
import {  Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule,CommonModule,MatFormFieldModule, MatInputModule, MatButtonModule,MatSelectModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {


    private user = new UserModel();
    public userForm: FormGroup;

    public constructor(
        private userService: UserService,
        private router: Router,
        private formBuilder: FormBuilder, 
        private notifyService: NotifyService
    ) { }


    ngOnInit(): void {
       this.userForm = this.formBuilder.group({
        nameControl: new FormControl("", [Validators.required, Validators.maxLength(50), Validators.minLength(2)]),
        emailControl: new FormControl("", [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(100),
            Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')
        ]),
        passwordControl: new FormControl("", [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(100),
            Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$')
        ]),
        roleControl: new FormControl("", [Validators.required])
       });
    }

    public async send(){
        if (this.userForm.valid) {
            try{
                this.user.name = this.userForm.get("nameControl").value;
                this.user.email = this.userForm.get("emailControl").value;
                this.user.password = this.userForm.get("passwordControl").value;
                this.user.roleId = this.userForm.get("roleControl").value;                        
                await this.userService.register(this.user);
                this.notifyService.success(`Welcome ${this.user.name}`);
                this.router.navigateByUrl("/home");
            }
            catch(err : any){
                this.notifyService.error(err);
            }
        }
    }



}
