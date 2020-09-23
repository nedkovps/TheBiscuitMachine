import React from 'react';
import './BiscuitBox.css';

const BiscuitBox = props => {
    return <div className="biscuit-box border border-dark bg-secondary float-right mt-3">
        <div className="biscuit-box-inner border border-dark border-top-0">
            <span>Total Biscuits Collected: {props.totalCount}</span>
        </div>
    </div>;
}

export default React.memo(BiscuitBox);