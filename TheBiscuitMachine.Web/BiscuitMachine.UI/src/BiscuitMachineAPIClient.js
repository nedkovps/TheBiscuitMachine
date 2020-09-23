export class BiscuitMachineAPIClient {
    static turnOn = async () => {
        await fetch('/api/BiscuitMachine/on', {
            method: 'POST',
            body: null
        });
    }

    static pause = async () => {
        await fetch('/api/BiscuitMachine/pause', {
            method: 'POST',
            body: null
        });
    }

    static turnOff = async () => {
        await fetch('/api/BiscuitMachine/off', {
            method: 'POST',
            body: null
        });
    }

    static getState = async () => {
        const stateResponse = await fetch('/api/BiscuitMachine/state', {
            method: 'GET',
            body: null
        });
        var state = stateResponse.json();
        return state;
    }
}