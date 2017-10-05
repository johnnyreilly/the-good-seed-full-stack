import * as React from 'react';
import { observer } from 'mobx-react';
import { ListerStore } from '../../../stores/listerStore';

export interface IListerProps {
    listerStore: ListerStore;
}

interface IState {
    text: string;
}

@observer
export class Lister extends React.Component<Partial<IListerProps>, IState> {
    state = {
        text: ''
    };

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { target: { value: text } } = event;
        this.setState(_prevState => ({ text }));
    }

    handleAdd = (_event: React.MouseEvent<HTMLButtonElement>) => {
        this.props.listerStore.add(this.state.text);
    }

    render() {
        const list = this.props.listerStore.list;
        return (
            <div>
                <h2>This is a lister</h2>
                <ul>
                    {list.map((entry, index) => <li key={index}>{entry}</li>)}
                </ul>
                <input type="text" value={this.state.text} onChange={this.handleChange} />
                <button onClick={this.handleAdd}>Add this to the list</button>
            </div>
        );
    }
}
