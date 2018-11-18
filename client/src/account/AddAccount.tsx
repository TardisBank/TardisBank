import * as React from 'react';

type AddAccountState = {
    isOpen: boolean
}


export class AddAccount extends React.Component<{},AddAccountState> {

    state = {isOpen: false}

    render() {
        return (
                <div>Add account {this.state.isOpen}</div>
        )
    }
}