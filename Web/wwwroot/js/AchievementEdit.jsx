class EditAchievementForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: this.props.achievement.name, description: this.props.achievement.description, condition: this.props.achievement.condition };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleConditionChange = this.handleConditionChange.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleConditionChange(e) {
        this.setState({ condition: e.target.value });
    }
    onCancelClick(e) {
        window.location.href = this.props.url + "?gameId=" + this.props.gameId;
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        const condition = this.state.condition.trim();

        const data = new FormData();
        data.append('Id', this.props.achievement.id);
        data.append('Name', name);
        data.append('Description', description);
        data.append('Condition', condition);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.updateUrl + "?gameId=" + this.props.gameId, true);
        xhr.onload = () => this.onCancelClick();
        xhr.send(data);
    }
    render() {
        return (
            <form className="editForm" onSubmit={this.handleSubmit} >
                <label>Achievement Name:</label><br />
                <input
                    type="text"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                /><br />
                <label>Achievement Description:</label><br />
                <textarea
                    rows="5"
                    cols="80"
                    type="text"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                /><br />
                <label>Achievement Condition:</label><br />
                <textarea
                    rows="5"
                    cols="80"
                    type="text"
                    value={this.state.condition}
                    onChange={this.handleConditionChange}
                /><br />
                <input type="submit" value="Save Achievement" />
                <button type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}