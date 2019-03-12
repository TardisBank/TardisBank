import * as React from "react";
import { WithStyles, Theme, createStyles } from "@material-ui/core";
import { TextField, Button, withStyles } from "@material-ui/core";
import green from "@material-ui/core/colors/green";
import { Direction } from "../model";

const styles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex"
    },

    deposit: {
      marginLeft: "4px",
      backgroundColor: green[800],

      "&:hover": {
        backgroundColor: green[900]
      }
    },

    withdraw: {
      marginLeft: "4px"
    }
  });

export type AddTransactionDispatchProps = {
  addTransaction: (amount: number, direction: Direction) => void;
};

export type AddTransactionState = {
  amount?: string;
  amountError: boolean;
};

export type AddTransactionProps = AddTransactionDispatchProps &
  WithStyles<typeof styles>;

class AddTransactionBase extends React.Component<AddTransactionProps> {
  state = {
    amount: undefined,
    amountError: false
  } as AddTransactionState;

  handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    this.setState({
      ...this.state,
      [name]: value
    });
  };

  private handleSubmit = (direction: Direction) => {
    const { amount } = this.state;
    if (!amount) {
      this.setState({
        ...this.state,
        amountError: true
      });
      return;
    }
    const value = parseFloat(amount);

    if (Number.isNaN(value) || value <= 0) {
      this.setState({
        ...this.state,
        amountError: true
      });
      return;
    }

    this.props.addTransaction(value, direction);
    this.setState({
      ...this.state,
      amountError: false,
      amount: undefined
    });
  };

  handleDeposit = this.handleSubmit.bind(this, Direction.Deposit);
  handleWithdrawal = this.handleSubmit.bind(this, Direction.Withdraw);

  render() {
    const { classes } = this.props;

    return (
      <main>
        <div className={classes.root}>
          <TextField
            autoComplete="off"
            name="amount"
            id="amount"
            label="Amount"
            fullWidth={true}
            required={true}
            error={this.state.amountError}
            value={this.state.amount || ""}
            onChange={this.handleChange}
          />
          <Button
            type="submit"
            color="primary"
            variant="raised"
            name="Save"
            onClick={this.handleDeposit}
            className={classes.deposit}
          >
            Deposit
          </Button>
          <Button
            type="submit"
            color="primary"
            variant="raised"
            name="Save"
            onClick={this.handleWithdrawal}
            className={classes.withdraw}
          >
            Withdraw
          </Button>
        </div>
      </main>
    );
  }
}

export const AddTransaction = withStyles(styles)(AddTransactionBase);
