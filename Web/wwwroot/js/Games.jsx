class FirstRow extends React.Component {
    render() {
        return (
            <thead>
                <tr>
                    <th class="text-center" scope="col">Name</th>
                    <th class="text-center" scope="col">Description</th>
                    <th class="text-center" scope="col">Rating</th>
                    <th class="text-center" scope="col">Genres</th>
                </tr>
            </thead>
        );
    }
}

class Rows extends React.Component {
    render() {
        return this.props.data.map(game => (
            <tr>
                <td class="text-center">{game.name}</td>
                <td class="text-center">{game.description}</td>
                <td class="text-center">{game.rating}</td>
                <td class="text-center">{game.genres}</td>
                <td><button class="btn btn-secondary" value={game.id} type="button" onClick={this.props.onAchievementsClick}>Achievements</button></td>
                <td><button class="btn btn-danger" value={game.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td><button class="btn btn-primary" value={game.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>
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
        this.state = { name: '', description: '', rating: '', genres: '' };
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
        this.setState({ name: '', description: '', rating: '', genres: '' });
    }
    render() {
        return (
            <form class="form" className="gameForm" align="center" onSubmit={this.handleSubmit}>
                <h3 class="d-flex">Create new game:</h3>
                <div class="form-group row">
                    <div class="col-lg-4 col-md-4 col-sm-4">
                        <input
                            type="text"
                            class="form-control"
                            placeholder="Game name"
                            value={this.state.name}
                            onChange={this.handleNameChange}
                        />
                        <input
                            type="text"
                            class="form-control"
                            placeholder="Game description"
                            value={this.state.description}
                            onChange={this.handleDescriptionChange}
                        />
                        <input
                            type="number"
                            class="form-control"
                            step="0.1"
                            min="0"
                            max="10"
                            placeholder="Game rating"
                            value={this.state.rating}
                            onChange={this.handleRatingChange}
                        />
                    </div>
                    <div class="d-flex col-12">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <select class="form-select" onClick={this.onOptionClick}>
                                <option disabled selected>Choose genres</option>
                                <GenreOption data={this.props.genres} />
                            </select>
                        </div>
                    </div>
                    <div class="d-flex col-12">
                        <input class="btn btn-primary" type="submit" value="Create Game" />
                    </div>
                </div>
            </form>
        );
    }
}

class GameParametersForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { sortBy: '', byDesc: '', searchBy: '', minRating: '', maxRating: '' };
        this.onSortOptionClick = this.onSortOptionClick.bind(this);
        this.onSortDescClick = this.onSortDescClick.bind(this);
        this.handleSearchByChange = this.handleSearchByChange.bind(this);
        this.handleMinRatingChange = this.handleMinRatingChange.bind(this);
        this.handleMaxRatingChange = this.handleMaxRatingChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    onSortOptionClick(e) {
        this.setState({ sortBy: e.target.value });
    }
    onSortDescClick(e) {
        this.setState({ byDesc: e.target.checked });
    }
    handleSearchByChange(e) {
        this.setState({ searchBy: e.target.value });
    }
    handleMinRatingChange(e) {
        this.setState({ minRating: e.target.value });
    }
    handleMaxRatingChange(e) {
        this.setState({ maxRating: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        let sortBy = this.state.sortBy.trim();
        if (this.state.byDesc == true) sortBy += " desc";
        const searchBy = this.state.searchBy;
        let minRating = this.state.minRating;
        if (minRating.includes(",")) {
            let nums = minRating.trim().split(",");
            minRating = (nums[0] + "." + nums[1]);
        }
        let maxRating = this.state.maxRating;
        if (maxRating.includes(",")) {
            let nums = maxRating.trim().split(",");
            maxRating = (nums[0] + "." + nums[1]);
        }
        this.props.loadGameOptions({ sortBy: sortBy, searchBy: searchBy, minRating: minRating, maxRating: maxRating });
    }
    render() {
        return (
            <form className="gameParametersForm" onSubmit={this.handleSubmit} >
                <div class="form-group row">
                    <label class="col-lg-1 col-md-2 col-sm-2 col-form-label text-center mt-1">Sort by:</label>
                    <div class="d-flex col-lg-3 col-md-3 col-sm-3">
                        <select class="form-select" onClick={this.onSortOptionClick}>
                            <option disabled selected>Choose field</option>
                            <option value="name">Name</option>
                            <option value="description">Description</option>
                            <option value="rating">Rating</option>
                        </select>
                    </div>
                    <input class="form-check-input mt-3" type="checkbox" value="" id="sortDesc" onClick={this.onSortDescClick} />
                    <label class="form-check-label col-lg-2 col-md-2 col-sm-2 col-form-label text-center mt-1" for="sortDesc">By descending</label>
                    <div class="col-12 row">
                        <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Search by name:</label>
                        <input
                            type="text"
                            class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                            placeholder="Name"
                            value={this.state.searchBy}
                            onChange={this.handleSearchByChange}
                        />
                    </div>
                    <div class="col-12 row">
                        <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Min rating:</label>
                        <input
                            type="number"
                            step="0.1"
                            min="0"
                            max="10"
                            class="d-flex col-lg-2 col-md-2 col-sm-2 mt-1"
                            placeholder="Min rating"
                            value={this.state.minRating}
                            onChange={this.handleMinRatingChange}
                        />
                        <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Max rating:</label>
                        <input
                            type="number"
                            step="0.1"
                            min="0"
                            max="10"
                            class="d-flex col-lg-2 col-md-2 col-sm-2 mt-1"
                            placeholder="Max rating"
                            value={this.state.maxRating}
                            onChange={this.handleMaxRatingChange}
                        />
                    </div>
                    <div class="col-12 row">
                        <label class="col-lg-1 col-md-1 col-sm-12 col-form-label">
                        </label>
                        <button class="btn btn-primary col-lg-2 col-md-2 col-sm-2 mt-4" type="submit">Accept</button>
                    </div>
                </div>
            </form>
        );
    }
}

class Table extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData, options: '' };
        this.handleGameSubmit = this.handleGameSubmit.bind(this);
        this.onGameDelete = this.onGameDelete.bind(this);
        this.onGameEdit = this.onGameEdit.bind(this);
        this.onAchievementsClick = this.onAchievementsClick.bind(this);
        this.loadGameOptions = this.loadGameOptions.bind(this);
    }
    loadGamesFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + this.state.options, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }
    onGameDelete(e) {
        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.deleteUrl + "?id=" + e.target.value, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send();
    }
    onGameEdit(e) {
        var url = this.props.editUrl + "?id=" + e.target.value;
        window.location.href = url;
    }
    onAchievementsClick(e) {
        var url = this.props.achievementsUrl + "?id=" + e.target.value;
        window.location.href = url;
    }
    loadGameOptions(options) {
        let optionsStr = "";
        if (!options.sortBy) optionsStr = "?orderBy=name";
        else optionsStr = "?orderBy=" + options.sortBy;
        if (options.searchBy) optionsStr += "&searchTerm=" + options.searchBy;
        if (options.minRating) optionsStr += "&minRating=" + options.minRating;
        if (options.maxRating) optionsStr += "&maxRating=" + options.maxRating;
        this.setState({ options: optionsStr });
    }
    handleGameSubmit(game, genres) {
        const data = new FormData();
        data.append('Name', game.name);
        data.append('Description', game.description);
        data.append('Rating', game.rating);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.creationUrl, true);
        xhr.onload = () => this.loadGamesFromServer();
        xhr.send(data);
        const xhrNew = new XMLHttpRequest();
        xhrNew.open('post', this.props.addGenresUrl + "?genreIds=" + genres + "&gameId=0", true);
        xhrNew.send();
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
                <GameParametersForm loadGameOptions={this.loadGameOptions} />
                <table class="table table-bordered">
                    <FirstRow />
                    <tbody>
                        <Rows data={this.state.data} onDeleteClick={this.onGameDelete} onEditClick={this.onGameEdit} onAchievementsClick={this.onAchievementsClick} />
                    </tbody>
                </table>
                <GameForm onGameSubmit={this.handleGameSubmit} genres={this.props.genresData} />
            </div>
        );
    }
}