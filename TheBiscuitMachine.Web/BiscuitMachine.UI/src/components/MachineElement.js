import React from 'react';
import classNames from 'classnames';
import './MachineElement.css';

const MachineElement = props => {

    const capacity = parseInt(props.capacity);

    const classes = classNames({
        'col-2': capacity === 1,
        'col-4': capacity === 2,
        'card': !props.empty,
        'p-0': !props.empty,
        'rounded-0': !props.empty
    });

    return <div className={classes}>
        {!props.empty && <div className="card-header">
            {props.name}
            <div className="float-right rounded-circle indicator" style={{ backgroundColor: props.isOn ? 'green' : 'red' }}></div>
        </div>}
        {!props.empty && <div className="card-body">
            {props.children}
        </div>}
    </div>;
}

export default React.memo(MachineElement);