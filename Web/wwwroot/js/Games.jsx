const data = [{ "id": 1, "name": "Dark Souls", "description": "Hardcore dark fantasy.", "rating": 9.3, "genres": "Dark fantasy, Hard game, RPG, Open world, Action" }, { "id": 3, "name": "DOOM", "description": "Fast shooter where you can take out your anger on demons", "rating": 9.4, "genres": "Action, Shooter" }, { "id": 2, "name": "The Witcher", "description": "Fantasy action about monster slayer.", "rating": 9.7, "genres": "RPG, Open world, Action" }]

class FirstRow extends React.Component {
    render() {
        return (
            <tr>
                <td align="center">Name</td>
                <td align="center">Description</td>
                <td align="center">Rating</td>
                <td align="center">Genres</td>
            </tr>
        );
    }
}

class Rows extends React.Component {
    render() {
        return this.props.data.map(game => (
            <tr>
                <td align="center">{game.name}</td>
                <td align="center">{game.description}</td>
                <td align="center">{game.rating}</td>
                <td align="center">{game.genres}</td>
            </tr>
        ));

    }
}

class GameForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', description: '', rating: 0.0 };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleRatingChange = this.handleRatingChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleRatingChange(e) {
        this.setState({ rating: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        const rating =this.state.rating.trim();
        /*if (!name || !description || !rating) {
            return;
        }*/
        this.props.onGameSubmit({ name: name, description: description, rating: rating });
        this.setState({ name: '', description: '', rating: 0.0 });
    }
    render() {
        return (
            <form className="gameForm" onSubmit={this.handleSubmit}>
                <input
                    type="text"
                    placeholder="Game name"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                />
                <input
                    type="text"
                    placeholder="Game description"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                />
                <input
                    type="number"
                    step="0.1"
                    lang="nb"
                    min="0"
                    max="10"
                    placeholder="Game rating"
                    value={this.state.rating}
                    onChange={this.handleRatingChange}
                />
                <input type="submit" value="Create Game" />
            </form>
        );
    }
}

class Table extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.handleGameSubmit = this.handleGameSubmit.bind(this);
    }
    loadGamesFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }
    handleGameSubmit(game) {
        const data = new FormData();
        data.append('Name', game.name);
        data.append('Description', game.description);
        data.append('Rating', game.rating);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
    }
    componentDidMount() {
        window.setInterval(
            () => this.loadGamesFromServer(),
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div className="table">
                <table width="80%" border="1" align="center">
                    <FirstRow />
                    <Rows data={this.state.data} />
                </table>
                <GameForm onGameSubmit={this.handleGameSubmit} />
            </div>
        );
    }
}