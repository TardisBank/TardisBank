import * as React from "react";
import { storiesOf } from "@storybook/react";
import { Register, RegisterProcess } from "./RegistrationForm";
import { RegistrationSent } from "./RegistrationSent";

const noop = () => {};

storiesOf("Registration", module).add("Registration form", () => (
  <Register
    registerProcess={RegisterProcess.Ready}
    onRegister={noop}
    toggleSignIn={noop}
  />
));

storiesOf("Registration", module).add("Registration sent", () => (
  <RegistrationSent />
));
