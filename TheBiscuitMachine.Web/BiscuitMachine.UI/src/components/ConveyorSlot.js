import React from 'react';
import classNames from 'classnames';
import './ConveyorSlot.css';

const ConveyorSlot = props => {

    const isEmpty = props.slot === 'Empty';

    const classes = classNames({
        'col-2': true,
        'occupied-slot': !isEmpty,
        'extracted': props.slot === 'Extracted' || props.slot === 'ReadyForStamp',
        'not-baked': props.slot === 'Stamped' || props.slot === 'ReadyToBake',
        'half-baked': props.slot === 'HalfBaked',
        'baked': props.slot === 'Baked' || props.slot === 'ReadyToCollect'
    });

    return <div className={classes}>
        <div className="slot">
            {!isEmpty && props.slot}
        </div>
    </div>;
}

export default React.memo(ConveyorSlot);