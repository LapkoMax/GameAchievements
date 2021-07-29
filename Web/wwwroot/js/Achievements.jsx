class FirstAchievementRow extends React.Component {
    render() {
        return (
            <thead key="0" align="center">
                <tr>
                    <th class="text-center" scope="col">Name</th>
                    <th class="text-center" scope="col">Description</th>
                    <th class="text-center" scope="col">Condition</th>
                </tr>
            </thead>
        );
    }
}

class AchievementRows extends React.Component {
    render() {
        return this.props.data.map(achievement => (
            <tr>
                <td class="text-center">{achievement.name}</td>
                <td class="text-center">{achievement.description}</td>
                <td class="text-center">{achievement.condition}</td>
                <td class="text-center"><button class="btn btn-danger" value={achievement.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td class="text-center"><button class="btn btn-primary" value={achievement.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>
            </tr>
        ));

    }
}

class AchievementForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', description: '', condition: '' };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleConditionChange = this.handleConditionChange.bind(this);
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
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        const condition = this.state.condition.trim();
        if (!name || !description || !condition) {
            return;
        }
        this.props.onGenreSubmit({ name: name, description: description, condition: condition });
        this.setState({ name: '', description: '', condition: '' });
    }
    render() {
        return (
            <form class="form" className="achievementForm" align="center" onSubmit={this.handleSubmit}>
                <h3 class="d-flex">Create new achievement:</h3>
                <div class="form-group row col-lg-4 col-md-4 col-sm-4">
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement name"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement description"
                        value={this.state.description}
                        onChange={this.handleDescriptionChange}
                    />
                    <input
                        type="text"
                        class="form-control"
                        placeholder="Achievement condition"
                        value={this.state.condition}
                        onChange={this.handleConditionChange}
                    />
                </div>
                <div class="d-flex row col-lg-2 col-md-3 col-sm-3">
                    <input class="btn btn-outline-primary" type="submit" value="Create Achievement" />
                </div>
            </form>
        );
    }
}

class AchievementsTable extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.handleAchievementSubmit = this.handleAchievementSubmit.bind(this);
        this.onAchievementDelete = this.onAchievementDelete.bind(this);
        this.onAchievementEdit = this.onAchievementEdit.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
    }
    loadAchievementsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + "?gameId=" + this.props.gameId, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }
    onAchievementDelete(e) {
        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.deleteUrl + "?gameId=" + this.props.gameId + "&achievementId=" + e.target.value, true);
        xhr.onload = () => this.loadAchievementsFromServer();
        xhr.send();
    }
    onAchievementEdit(e) {
        var url = this.props.editUrl + "?gameId=" + this.props.gameId + "&id=" + e.target.value;
        window.location.href = url;
    }
    onCancelClick() {
        window.location.href = this.props.cancelUrl;
    }
    handleAchievementSubmit(achievement) {
        const data = new FormData();
        data.append('Name', achievement.name);
        data.append('Description', achievement.description);
        data.append('Condition', achievement.condition);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl + "?gameId=" + this.props.gameId, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
    }
    componentDidMount() {
        window.setInterval(
            () => this.loadAchievementsFromServer(),
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div className="table">
                <table class="table table-bordered">
                    <FirstAchievementRow />
                    <tbody>
                        <AchievementRows data={this.state.data} onDeleteClick={this.onAchievementDelete} onEditClick={this.onAchievementEdit} />
                    </tbody>
                </table>
                <AchievementForm onGenreSubmit={this.handleAchievementSubmit} />
                <div class="col-12 row">
                    <button class="btn btn-secondary col-lg-2 col-md-2 col-sm-2 mt-4" type="button" onClick={this.onCancelClick} >Cancel</button>
                </div>
            </div>
        );
    }
}