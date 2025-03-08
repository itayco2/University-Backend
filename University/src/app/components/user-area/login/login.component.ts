import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CredentialsModel } from '../../../models/credentials.model';
import { UserService } from '../../../services/user.service';
import { Router } from '@angular/router';
import { NotifyService } from '../../../services/notify.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, MatFormFieldModule, MatInputModule, MatCheckboxModule, MatButtonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']  // Fixed typo: changed styleUrl to styleUrls
})
export class LoginComponent implements OnInit {

    private credentials = new CredentialsModel();
    public credentialsForm: FormGroup;
    

    constructor(
        private userService: UserService,
        private router: Router,
        private formBuilder: FormBuilder,
        private notifyService: NotifyService
    ) { }

    ngOnInit(): void {
        this.credentialsForm = this.formBuilder.group({
            emailControl: new FormControl("", [Validators.required, Validators.email]),  // Added validators
            passwordControl: new FormControl("", Validators.required)  // Added validators
        });
    }

    public async send() {
        if (this.credentialsForm.invalid) {
            this.notifyService.error('Please fill in all fields correctly.');
            return;
        }

        try {
            this.credentials.email = this.credentialsForm.get("emailControl").value;
            this.credentials.password = this.credentialsForm.get("passwordControl").value;

            await this.userService.login(this.credentials);  // Ensure login method is returning a Promise
            this.notifyService.success('Welcome Back!');
            this.router.navigateByUrl("/home");
        }
        catch (err: any) {
            this.notifyService.error(err);
        }
    }
}
