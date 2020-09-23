import React from 'react';

const MachineElements = props => {
    return <div className="row mt-3 ml-0" style={{ width: '1200px', height: '120px' }}>
        {props.children}
    </div>;
}

export default React.memo(MachineElements);