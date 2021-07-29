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
            <form class="form-control" className="editForm" onSubmit={this.handleSubmit} >
                <div class="col-12 row">
                    <label class="col-lg-2 col-md-4 col-sm-4 col-form-label text-center mt-1">Achievement Name:</label>
                    <input
                        type="text"
                        class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                </div>
                <div class="col-12 row">
                    <label class="col-lg-2 col-md-4 col-sm-4 col-form-label text-center mt-1">Achievement Description:</label>
                    <div class="col-lg-10 col-md-8 col-sm-8">
                        <textarea
                            class="form-control"
                            rows="5"
                            type="text"
                            value={this.state.description}
                            onChange={this.handleDescriptionChange}
                        />
                    </div>
                </div>
                <div class="col-12 row">
                    <label class="col-lg-2 col-md-4 col-sm-4 col-form-label text-center mt-1">Achievement Condition:</label>
                    <div class="col-lg-10 col-md-8 col-sm-8">
                        <textarea
                            class="form-control"
                            rows="5"
                            type="text"
                            value={this.state.condition}
                            onChange={this.handleConditionChange}
                        />
                    </div>
                </div>
                <button class="btn btn-primary" type="submit">Save Achievement</button>
                <button className="btn btn-secondary" type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}