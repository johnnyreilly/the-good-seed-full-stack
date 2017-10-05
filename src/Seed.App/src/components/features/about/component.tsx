import * as React from 'react';
import { inject } from 'mobx-react';
import { IListerProps, Lister } from '../../shared/lister/component';
import { Stores } from '../../../stores';
import { Counter } from './counter/component';

const Lister1 = inject(
    (stores: Stores) => ({
        listerStore: stores.lister1Store
    }) as IListerProps
)(Lister);

const Lister2 = inject(
    (stores: Stores) => ({
        listerStore: stores.lister2Store
    }) as IListerProps
)(Lister);

export class AboutPage extends React.Component {
    render() {
        return (
            <div>
                <h2>About</h2>
                <Counter />
                <h2>Lister 1</h2>
                <Lister1 />
                <h2>Lister 2</h2>
                <Lister2 />
            </div>
        );
    }
}
