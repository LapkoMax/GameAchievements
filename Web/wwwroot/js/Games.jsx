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
                <td class="text-center"><button class="btn btn-secondary" value={game.id} type="button" onClick={this.props.onAchievementsClick}>Achievements</button></td>
                <td class="text-center"><button class="btn btn-danger" value={game.id} type="button" onClick={this.props.onDeleteClick}>Delete</button></td>
                <td class="text-center"><button class="btn btn-primary" value={game.id} type="button" onClick={this.props.onEditClick}>Edit</button></td>
            </tr>
        ));

    }
}

class GenreOption extends React.Component {
    render() {
        return this.props.data.map(genre => (
            <option value={genre.id} title={genre.description}>{ genre.name }</ option>
        ));
    }
}

class GenresToAddForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { genreIds: '', genreEntities: [] };
        this.updateGenreEntities = this.updateGenreEntities.bind(this);
    }
    updateGenreEntities(genreIds) {
        var ids = genreIds.split(' ');
        var genreEntities = [];
        ids.forEach(id => {
            this.props.genres.forEach(genre => {
                if (id == genre.id) genreEntities.push(genre);
            });
        });
        this.setState({ genreIds: genreIds, genreEntities: genreEntities });
    }
    componentWillReceiveProps(newProps) {
        if (this.state.genreIds.trim() != newProps.genreIds) this.updateGenreEntities(newProps.genreIds);
    }
    render() {
        return this.state.genreEntities.map(genre => (
                <div class="col-lg-3 col-md-3 col-sm-3 row">
                    <label class="text-center col-lg-6 col-md-6 col-sm-6 ml-2">{genre.name}</label>
                    <button class="btn btn-danger col-lg-6 col-md-6 col-sm-6 mt-1 mb-1" value={genre.id} onClick={this.props.onGenreToAddDelte}>Delete</button>
                </div>
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
        this.onGenreToAddDelte = this.onGenreToAddDelte.bind(this);
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
        if (isAdd) this.setState({ genres: this.state.genres + e.target.value + ' ' });
    }
    onGenreToAddDelte(e) {
        var ids = this.state.genres.split(' ');
        var newIds = "";
        ids.forEach(id => {
            if (id != e.target.value) newIds += id + " ";
        });
        this.setState({ genres: newIds.trim() });
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
                        <div class="col-lg-9 col-md-9 col-sm-9 row">
                            <GenresToAddForm genres={this.props.genres} genreIds={this.state.genres} onGenreToAddDelte={this.onGenreToAddDelte} />
                        </div>
                    </div>
                    <div class="d-flex col-12">
                        <input class="btn btn-outline-primary" type="submit" value="Create Game" />
                    </div>
                </div>
            </form>
        );
    }
}

class GamePager extends React.Component {
    constructor(props) {
        super(props);
        this.state = { currentPage: this.props.metaData.currentPage, page: '', pageSize: '' };
        this.handlePageChange = this.handlePageChange.bind(this);
        this.onGoClick = this.onGoClick.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onChangeClick = this.onChangeClick.bind(this);
        this.handlePageClick = this.handlePageClick.bind(this);
    }
    handlePageChange(e) {
        this.setState({ page: e.target.value });
    }
    onGoClick() {
        var currentPage = this.state.page;
        this.props.loadGamePageOptions({ currentPage: currentPage });
    }
    onPageSizeChange(e) {
        this.setState({ pageSize: e.target.value });
    }
    onChangeClick() {
        var newPageSize = this.state.pageSize;
        this.props.changePageSize({ pageSize: newPageSize });
    }
    handlePageClick(e) {
        var currentPage = this.state.currentPage;
        if (e.target.value == "Prev") {
            this.setState({ currentPage: +this.state.currentPage - 1 });
            currentPage -= 1;
        }
        else if (e.target.value == "Next") {
            this.setState({ currentPage: +this.state.currentPage + 1 });
            currentPage += 1;
        }
        else {
            this.setState({ currentPage: e.target.value });
            currentPage = e.target.value;
        }
        this.props.loadGamePageOptions({ currentPage: currentPage });
    }
    render() {
        var pageNums = [];
        var components = [];
        for (let i = 1; i <= parseInt(this.props.metaData.totalPages); i++) {
            pageNums.push(i);
        }
        if (this.props.metaData.hasPrevious) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick}>&laquo;</button>);
        else components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Prev" onClick={this.handlePageClick} disabled>&laquo;</button>);
        pageNums.map(num => {
            if (num == 1 && num != this.props.metaData.currentPage) {
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
                if (this.props.metaData.currentPage > 3 && this.props.metaData.totalPages >= 5) components.push(<label class="text-center col-lg-1 col-mg-1 col-sm-1">...</label>);
            }
            else if (num == this.props.metaData.totalPages && num != this.props.metaData.currentPage) {
                if (this.props.metaData.currentPage < this.props.metaData.totalPages - 2 && this.props.metaData.totalPages >= 5) components.push(<label class="text-center col-lg-1 col-mg-1 col-sm-1">...</label>);
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            }
            else if (num == this.props.metaData.currentPage)
                components.push(<button class="btn btn-primary col-lg-1 col-mg-1 col-sm-1" disabled>{num}</button>);
            else if (num == this.props.metaData.currentPage - 1 || num == this.props.metaData.currentPage + 1)
                components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            else {
                if (this.props.metaData.totalPages < 5) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value={num} onClick={this.handlePageClick}>{num}</button>);
            }
        })
        if (this.props.metaData.hasNext) components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Next" onClick={this.handlePageClick}>&raquo;</button>);
        else components.push(<button class="btn btn-outline-primary col-lg-1 col-mg-1 col-sm-1" value="Next" onClick={this.handlePageClick} disabled>&raquo;</button>);
        return (
            <form class="text-center form col-12 row">
                <div class="col-12 row">
                    <div class="col-lg-5 col-md-5 col-sm-5">
                        {components.map(component => (
                            component
                        ))}
                    </div>
                    <div class="col-lg-1 col-md-2 col-sm-2 mt-1 row"><input
                        type="number"
                        step="1"
                        min="1"
                        max={this.props.metaData.totalPages}
                        class="form-control"
                        placeholder="Page"
                        value={this.state.page}
                        onChange={this.handlePageChange}
                    /></div>
                    <button class="btn btn-primary col-lg-1 col-mg-1 col-sm-1 mt-1" onClick={this.onGoClick}>Go</button>
                    <div class="col-lg-2 col-md-2 col-sm-2 mt-1 row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <select class="form-select" onClick={this.onPageSizeChange}>
                                <option disabled selected>Page size</option>
                                <option value="10">10</option>
                                <option value="25">25</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                                <option value="150">150</option>
                                <option value="200">200</option>
                            </select>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3"><button class="btn btn-primary" onClick={this.onChangeClick}>Change</button></div>
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
                    <label class="d-flex col-lg-2 col-md-2 col-sm-2 col-form-label text-center mt-1">Sort by:</label>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <select class="form-select" onClick={this.onSortOptionClick}>
                            <option disabled selected>Choose field</option>
                            <option value="name">Name</option>
                            <option value="description">Description</option>
                            <option value="rating">Rating</option>
                        </select>
                    </div>
                    <input class="form-check-input mt-3" type="checkbox" value="" id="sortDesc" onClick={this.onSortDescClick} />
                    <label class="form-check-label col-lg-2 col-md-2 col-sm-2 col-form-label text-left mt-1" for="sortDesc">By descending</label>
                    <div class="col-12 row">
                        <label class="d-flex col-lg-2 col-md-2 col-sm-2 col-form-label text-center mt-1">Search by name:</label>
                        <input
                            type="text"
                            class="col-lg-3 col-md-3 col-sm-3 mt-1"
                            placeholder="Name"
                            value={this.state.searchBy}
                            onChange={this.handleSearchByChange}
                        />
                    </div>
                    <div class="col-12 row">
                        <label class="d-flex col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Min rating:</label>
                        <input
                            type="number"
                            step="0.1"
                            min="0"
                            max="10"
                            class="d-flex col-lg-1 col-md-1 col-sm-1 mt-1 text-truncate"
                            placeholder="Min rating"
                            value={this.state.minRating}
                            onChange={this.handleMinRatingChange}
                        />
                        <label class="col-lg-1 col-md-2 col-sm-2 col-form-label text-center mt-1">Max rating:</label>
                        <input
                            type="number"
                            step="0.1"
                            min="0"
                            max="10"
                            class="d-flex col-lg-1 col-md-1 col-sm-1 mt-1 text-truncate"
                            placeholder="Max rating"
                            value={this.state.maxRating}
                            onChange={this.handleMaxRatingChange}
                        />
                    </div>
                    <div class="col-12 row">
                        <button class="btn btn-primary col-lg-2 col-md-2 col-sm-2" type="submit">Accept</button>
                    </div>
                    <GamePager metaData={this.props.metaData} loadGamePageOptions={this.props.loadGamePageOptions} changePageSize={this.props.changePageSize} />
                </div>
            </form>
        );
    }
}

class Table extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData, options: '', metaData: this.props.metaData, pageNumber: this.props.metaData.pageNumber, pageSize: this.props.metaData.pageSize };
        this.handleGameSubmit = this.handleGameSubmit.bind(this);
        this.onGameDelete = this.onGameDelete.bind(this);
        this.onGameEdit = this.onGameEdit.bind(this);
        this.onAchievementsClick = this.onAchievementsClick.bind(this);
        this.loadGameOptions = this.loadGameOptions.bind(this);
        this.loadGamePageOptions = this.loadGamePageOptions.bind(this);
        this.changePageSize = this.changePageSize.bind(this);
    }
    loadGamesFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url + this.state.options, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();

        const newXhr = new XMLHttpRequest();
        newXhr.open('get', this.props.getMetaDataUrl + this.state.options, true);
        newXhr.onload = () => {
            const metaData = JSON.parse(newXhr.responseText);
            this.setState({ metaData: metaData });
        };
        newXhr.send();
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
        var validRatingRange = true;
        if (options.minRating && options.maxRating && options.maxRating < options.minRating) validRatingRange = false;
        if (options.minRating && validRatingRange) optionsStr += "&minRating=" + options.minRating;
        if (options.maxRating && validRatingRange) optionsStr += "&maxRating=" + options.maxRating;
        optionsStr += "&pageNumber=" + this.state.pageNumber;
        optionsStr += "&pageSize=" + this.state.pageSize;
        this.setState({ options: optionsStr });
    }
    loadGamePageOptions(options) {
        if (options.currentPage) this.setState({ pageNumber: options.currentPage });
    }
    changePageSize(options) {
        if (options.pageSize) this.setState({ pageSize: options.pageSize });
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
            this.props.pollInternal,
        );
    }
    render() {
        return (
            <div className="table">
                <GameParametersForm loadGameOptions={this.loadGameOptions} loadGamePageOptions={this.loadGamePageOptions} changePageSize={this.changePageSize} metaData={this.state.metaData}/>
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