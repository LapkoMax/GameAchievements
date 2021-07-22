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
                <td align="center"><button value={game.id} type="submit" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td align="center"><button value={game.id} type="submit" onClick={this.props.onEditClick}>Edit</button></td>
            </tr>
        ));

    }
}

class GenreOption extends React.Component {
    render() {
        return this.props.data.map(genre => (
            <option value={genre.id}>{genre.name}</option>
        ));
    }
}

class GameForm extends React.Component {
    constructor(props) {
        super(props);    
        this.state = { name: '', description: '', rating: '', genres: ''};
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleRatingChange = this.handleRatingChange.bind(this);
        this.onOptionClick = this.onOptionClick.bind(this);
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
    onOptionClick(e) {
        if (e.target.value.includes("Choose")) return;
        let ids = this.state.genres.split(' ');
        let isAdd = true;
        ids.forEach(id => { if (id == e.target.value) isAdd = false });
        if (isAdd) this.state.genres += e.target.value + ' ';
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        let ratingStr = this.state.rating;
        if (this.state.rating.includes(".")) {
            let nums = this.state.rating.trim().split(".");
            ratingStr = (nums[0] + "," + nums[1]);
        }
        const rating = ratingStr;
        const genres = this.state.genres.trim();
        if (!name || !description || !rating) {
            return;
        }
        this.props.onGameSubmit({ name: name, description: description, rating: rating }, genres);
        this.setState({ name: '', description: '', rating: '', genres: ''});
    }
    render() {
        return (
            <form className="gameForm" align="center" onSubmit={this.handleSubmit}>
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
                    min="0"
                    max="10"
                    placeholder="Game rating"
                    value={this.state.rating}
                    onChange={this.handleRatingChange}
                />
                <select id="Genres" onClick={this.onOptionClick}>
                    <option disabled selected>Choose genres</option>
                    <GenreOption data={this.props.genres}/>
                </select>
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
        this.onGameDelete = this.onGameDelete.bind(this);
        this.onGameEdit = this.onGameEdit.bind(this);
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
    onGameDelete(e) {
        const data = new FormData();
        data.append('GameId', e.target.value);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.deleteUrl, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
    }
    onGameEdit(e) {
        const data = new FormData();
        data.append('GameId', e.target.value);

        /*var url = this.props.editUrl + "?id=" + e.target.value;
        window.location = url;*/

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.editUrl, true);
        xhr.send(data);              // How to change View to EditGame?
    }
    handleGameSubmit(game, genres) {
        const data = new FormData();
        data.append('Name', game.name);
        data.append('Description', game.description);
        data.append('Rating', game.rating);

        const transferData = new FormData();
        transferData.append('GenreIds', genres);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);

        const xhrNew = new XMLHttpRequest();
        xhrNew.open('post', this.props.addGenresUrl, true);
        xhrNew.send(transferData);
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
                    <Rows data={this.state.data} onDeleteClick={this.onGameDelete} onEditClick={this.onGameEdit} />
                </table>
                <GameForm onGameSubmit={this.handleGameSubmit} genres={this.props.genresData} />
            </div>
        );
    }
}