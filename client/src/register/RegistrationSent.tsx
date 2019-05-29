import * as React from "react";
import {
  Paper,
  Avatar,
  Typography,
  WithStyles,
  withStyles
} from "@material-ui/core";
import { ThumbUp } from "@material-ui/icons";
import { styles } from "./RegistrationForm.styles";

const _RegistrationSent = ({ classes }: WithStyles<typeof styles>) => (
  <main className={classes.layout}>
    <Paper className={classes.paper}>
      <Avatar className={classes.thumbsUp}>
        <ThumbUp />
      </Avatar>
      <Typography variant="headline">Regsitration was successful</Typography>
      <Typography variant="subheading">
        An email will arrive with instructions for completing the registration
        process.
      </Typography>
    </Paper>
  </main>
);

export const RegistrationSent = withStyles(styles)(_RegistrationSent);
