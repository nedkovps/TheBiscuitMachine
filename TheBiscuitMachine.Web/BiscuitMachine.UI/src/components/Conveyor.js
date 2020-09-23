import React from 'react';
import ConveyorSlot from './ConveyorSlot';
import './Conveyor.css';

const Conveyor = props => {
    return <div>
        <div className="conveyor bg-secondary"></div>
        <div className="row conveyor-wrapper" style={{ marginLeft: `${props.ratio * 200}px` }}>
            {props.slots.map((slot, i) => <ConveyorSlot key={i} slot={slot} />)}
        </div>
    </div>;
}

export default React.memo(Conveyor);